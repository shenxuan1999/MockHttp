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