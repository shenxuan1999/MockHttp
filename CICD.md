# ssh登录

在服务器配置一下ssh公钥，root用户在/root/.ssh/authorized_keys,有几个公钥就配置几个 每行一个

chmod 600 /root/.ssh/authorized_keys

```
ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABAQDJpl+XhuaMZfFR6mMu1tBG4DIfxQ8BcE2M5HLRFsJo7ymb/1KmzF990v4eg53ZsFrsyBy+EtabRHWqeaWn5PJl/ykMBU7fRjfPLtYs7p6KHwRqvOTk4DbUhdDnlrrjg4VuRtGf90bHxQnJ33rX37H703kokCNL7r+Ji67Pt/F0wV7WS+uYxX3CSf1CMHBdXJ/YpZdJJvnVUGiytGJqbBZ5GmnvpASAh60jD011SJFvYVP5WHvWn0p4eyPQLoSY+z32UYQjrexbC0VgeRq7Dqh3wpKWZPTZgQ/am6NWXN02cDCTfGmGFrFS4w/2VHECRd/JEwp1SgDV4kF5El2bbwdn a@DESKTOP-5VJ71O2

ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIOyVqz9YDTOC32NtgUu1aoZe7TDxDS9TPAAUKoTzi0jY shenxuan1999@gmail.com
```



服务器配置 /etc/ssh/sshd_config的PubkeyAuthentication yes 允许sshkey登陆

systemctl restart ssh.service



# 1.右键发布自动部署

项目csproj文件配置

```sh
  <PropertyGroup>
    <!-- 自动使用项目名称 -->
    <ProjectName>$(MSBuildProjectName)</ProjectName>

    <!-- 定义默认参数 -->
    <DeployUser>root</DeployUser>
    <DeployHost>38.76.195.112</DeployHost>
    <DeployAppDir>/service/$(ProjectName)</DeployAppDir>
    <DeployServiceName>$(ProjectName).service</DeployServiceName>
    <DeployPublishDir>./MockHttp/bin/Release/net8.0/publish</DeployPublishDir>
  </PropertyGroup>

  <Target Name="RunDeployAfterVSWebPublish" AfterTargets="AfterPublish">
    <Exec Command="deploy.bat $(DeployUser) $(DeployHost) $(DeployAppDir) $(DeployServiceName) $(DeployPublishDir)" WorkingDirectory="$(SolutionDir)" />
  </Target>

```

脚本

deploy.bat

```sh
@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion



REM ========== 默认配置 ==========
set "DEFAULT_USER=root"
set "DEFAULT_HOST=115.190.13.222"
set "DEFAULT_APP_DIR=/service/MockHttp"
set "DEFAULT_SERVICE_NAME=MockHttp.service"
set "DEFAULT_PUBLISH_DIR=MockHttp\bin\Release\net8.0\publish"

REM ========== 解析命令行参数 ==========
set "USER=%~1"
if "!USER!"=="" set "USER=!DEFAULT_USER!"

set "HOST=%~2"
if "!HOST!"=="" set "HOST=!DEFAULT_HOST!"

set "APP_DIR=%~3"
if "!APP_DIR!"=="" set "APP_DIR=!DEFAULT_APP_DIR!"

set "SERVICE_NAME=%~4"
if "!SERVICE_NAME!"=="" set "SERVICE_NAME=!DEFAULT_SERVICE_NAME!"

set "PUBLISH_DIR=%~5"
if "!PUBLISH_DIR!"=="" set "PUBLISH_DIR=!DEFAULT_PUBLISH_DIR!"

REM ========== 部署流程 ==========
echo 🚀 部署开始...
echo 📌 使用参数: USER=!USER!, HOST=!HOST!, APP_DIR=!APP_DIR!, SERVICE_NAME=!SERVICE_NAME!, PUBLISH_DIR=!PUBLISH_DIR!

if not exist "!PUBLISH_DIR!" (
    echo ❌ 错误：发布目录 "!PUBLISH_DIR!" 不存在，请先执行 dotnet publish
    goto :end
)

echo ⏹️ 停止远程服务：!SERVICE_NAME!
ssh !USER!@!HOST! "sudo systemctl stop !SERVICE_NAME!"

echo 📤 上传文件到服务器 !HOST!:!APP_DIR!
scp -r "!PUBLISH_DIR!\*" !USER!@!HOST!:!APP_DIR!/

echo ▶️ 启动远程服务：!SERVICE_NAME!
ssh !USER!@!HOST! "sudo systemctl start !SERVICE_NAME!"

echo ✅ 部署完成

:end
endlocal
```

deploy.sh

```sh
#!/bin/bash

# ========== 默认配置 ==========
DEFAULT_USER="root"
DEFAULT_HOST="115.190.13.222"
DEFAULT_APP_DIR="/service/MockHttp"
DEFAULT_SERVICE_NAME="MockHttp.service"
DEFAULT_PUBLISH_DIR="./MockHttp/bin/Release/net8.0/publish"

# ========== 解析命令行参数 ==========
USER=${1:-$DEFAULT_USER}
HOST=${2:-$DEFAULT_HOST}
APP_DIR=${3:-$DEFAULT_APP_DIR}
SERVICE_NAME=${4:-$DEFAULT_SERVICE_NAME}
PUBLISH_DIR=${5:-$DEFAULT_PUBLISH_DIR}

# ========== 部署流程 ==========
echo "🚀 部署开始..."
echo "📌 使用参数: USER=$USER, HOST=$HOST, APP_DIR=$APP_DIR, SERVICE_NAME=$SERVICE_NAME, PUBLISH_DIR=$PUBLISH_DIR"

if [ ! -d "$PUBLISH_DIR" ]; then
    echo "❌ 错误：发布目录 $PUBLISH_DIR 不存在，请先执行 dotnet publish"
    exit 1
fi

echo "⏹️ 停止远程服务：$SERVICE_NAME"
ssh $USER@$HOST "sudo systemctl stop $SERVICE_NAME"

echo "📤 上传文件到服务器 $HOST:$APP_DIR"
scp -r $PUBLISH_DIR/* $USER@$HOST:$APP_DIR/

echo "▶️ 启动远程服务：$SERVICE_NAME"
ssh $USER@$HOST "sudo systemctl start $SERVICE_NAME"

echo "✅ 部署完成"
```

# 2.GitHub Action

解决方案同级下新建.github/workflows/ci.yml

如果scp拷贝到服务器需要压缩解压，服务器需要提前安装解压缩软件 具体看依赖的脚本内容

```yaml
name: .NET 8 Deploy

env:
  PROJECT_NAME: MockHttp

on:
  push:
    branches: ["master"]

jobs:
  deploy:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 8.0.x

      - name: Publish WebApi
        run: |
          dotnet publish $PROJECT_NAME/$PROJECT_NAME.csproj \
            -c Release \
            -o publish \
            -p:DebugType=None \
            -p:DebugSymbols=false

      - name: Deploy to Server
        uses: appleboy/scp-action@v0.1.7
        with:
          host: ${{ secrets.SSH_HOST }}
          username: ${{ secrets.SSH_USER }}
          port: ${{ secrets.SSH_PORT }}
          key: ${{ secrets.SSH_PRIVATE_KEY }}
          source: "publish/*"
          target: "/service/MockHttp"
          strip_components: 1

      - name: Restart service
        uses: appleboy/ssh-action@v1.0.3
        with:
          host: ${{ secrets.SSH_HOST }}
          username: ${{ secrets.SSH_USER }}
          key: ${{ secrets.SSH_PRIVATE_KEY }}
          script: |
            systemctl restart MockHttp.service


```

> 对应的仓库需要设置变量 ip 端口 用户 私钥
>
> 仓库-   Settings-    secrets and variables   的Repository secrets
>
> 仓库变量针对仓库  环境变量针对环境
>
> Secrets的变量使用 ${{ secrets.SSH_HOST }}
>
> Variables的变量使用 ${{ vars.SSH_HOST }}



yml的里面的变量使用

| 场景            | 用法                      |
| --------------- | ------------------------- |
| `run:` bash     | `$PROJECT_NAME`           |
| `with:` 参数    | `${{ env.PROJECT_NAME }}` |
| `if:` 条件      | `${{ env.PROJECT_NAME }}` |
| job / step 名称 | `${{ env.PROJECT_NAME }}` |