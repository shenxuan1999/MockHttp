### 路径问题

PUBLISH_DIR都表示当前目录

在两种脚本中 `PUBLISH_DIR` 都表示**当前目录下的 `bin/Release/net8.0/publish` 文件夹**，但它们的写法因操作系统差异而有所不同：

------

**Linux (`deploy.sh`)**

bash

```
PUBLISH_DIR=./bin/Release/net8.0/publish
```

- `./` 表示**当前目录**（显式声明）。
- 路径分隔符为 `/`（Linux/Unix 风格）。
- 如果省略 `./`（直接写 `bin/...`），效果相同，因为默认会从当前目录开始解析。

------

**Windows (`deploy.bat`)**

bat

```
set "PUBLISH_DIR=bin\Release\net8.0\publish"
```

- 没有 `./` 前缀，但默认从**当前目录**开始解析（Windows 风格）。
- 路径分隔符为 `\`（Windows 风格），但实际也支持 `/`（如 `bin/Release/...`）。
- 如果路径以 `.\` 开头（如 `.\bin\...`），效果与 `./` 相同，但通常省略。



> 测试bat里面也可以使用./bin/Release/net8.0/publish   建议都统一为./bin/Release/net8.0/publish  或者bin/Release/net8.0/publish
>
> 只要不是绝对路径就行



## 使用

### 生成执行

项目  属性  生成 可以设置生成前执行的事件与生成后执行的事件

```shell
  <Target Name="PostBuild" AfterTargets="PostBuildEvent">
    <Exec Command="$(SolutionDir)deploy.bat" />
  </Target>
```

### 发布执行

**这个只有命令行可以执行**

```sh
<Target Name="PostPublish" AfterTargets="Publish">
  <Exec Command="$(SolutionDir)deploy.bat" />
</Target>
```

**右键可以执行；命令行也可以执行**

```
 <Target Name="RunDeployAfterVSWebPublish" AfterTargets="AfterPublish">
    <Exec Command="deploy.bat" />
  </Target>
```

命令行发布可以  为什么右键发布不行

------

**根本原因**

Visual Studio 的 **“右键发布”** 使用的是 *MSBuild 的 PublishProfile*，而不是 `dotnet publish` 命令行流程。因此，`csproj` 中的：

```
<Target Name="PostPublish" AfterTargets="Publish">
  <Exec Command="deploy.bat" />
</Target>
```

⚠️ **不会被自动调用**，因为 VS 发布流程不会触发这个 `Publish` target 的默认行为。

------

✅ 正确解决方式：使用 `AfterPublish` Hook

你可以修改 `csproj` 文件，绑定到 **Visual Studio 发布流程**会触发的目标点，例如：

```
xml复制编辑<Target Name="RunDeployAfterVSWebPublish" AfterTargets="AfterPublish">
  <Exec Command="deploy.bat" />
</Target>
```

这样就能兼容：

- ✅ `dotnet publish`
- ✅ Visual Studio 右键发布



## 变量执行

注意：自定义的变量不要与下面的变量说明重复  

```xml
 <PropertyGroup>
   <!-- 定义默认参数 -->
   <DeployUser>root</DeployUser>
   <DeployHost>115.190.13.222</DeployHost>
   <DeployAppDir>/service/WebApplicationDemo</DeployAppDir>
   <DeployServiceName>WebApplicationDemo.service</DeployServiceName>
   <DeployPublishDir>./bin/Release/net8.0/publish</DeployPublishDir>
 </PropertyGroup>

 <Target Name="RunDeployAfterVSWebPublish" AfterTargets="AfterPublish">
   <Exec Command="deploy.sh $(DeployUser) $(DeployHost) $(DeployAppDir) $(DeployServiceName) $(DeployPublishDir)" />
 </Target>
```

### deploy.sh

```sh
#!/bin/bash

# ========== 默认配置 ==========
DEFAULT_USER="root"
DEFAULT_HOST="115.190.13.222"
DEFAULT_APP_DIR="/service/WebApplicationDemo"
DEFAULT_SERVICE_NAME="WebApplicationDemo.service"
DEFAULT_PUBLISH_DIR="./bin/Release/net8.0/publish"

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

### deploy.bat

```xml
 <PropertyGroup>
   <!-- 定义默认参数 -->
   <DeployUser>root</DeployUser>
   <DeployHost>115.190.13.222</DeployHost>
   <DeployAppDir>/service/WebApplicationDemo</DeployAppDir>
   <DeployServiceName>WebApplicationDemo.service</DeployServiceName>
   <DeployPublishDir>bin\Release\net8.0\publish</DeployPublishDir>
 </PropertyGroup>

 <Target Name="RunDeployAfterVSWebPublish" AfterTargets="AfterPublish">
   <Exec Command="deploy.bat $(DeployUser) $(DeployHost) $(DeployAppDir) $(DeployServiceName) $(DeployPublishDir)" />
 </Target>
```

```sh
@echo off
chcp 65001 >nul
setlocal enabledelayedexpansion

REM ========== 默认配置 ==========
set "DEFAULT_USER=root"
set "DEFAULT_HOST=115.190.13.222"
set "DEFAULT_APP_DIR=/service/WebApplicationDemo"
set "DEFAULT_SERVICE_NAME=WebApplicationDemo.service"
set "DEFAULT_PUBLISH_DIR=bin\Release\net8.0\publish"

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

### 命令行覆盖参数

```xml
<PropertyGroup>
  <!-- 定义默认参数（可通过命令行覆盖） -->
  <DeployUser Condition="'$(DeployUser)' == ''">root</DeployUser>
  <DeployHost Condition="'$(DeployHost)' == ''">115.190.13.222</DeployHost>
  <DeployAppDir Condition="'$(DeployAppDir)' == ''">/service/WebApplicationDemo</DeployAppDir>
  <DeployServiceName Condition="'$(DeployServiceName)' == ''">WebApplicationDemo.service</DeployServiceName>
  <DeployPublishDir Condition="'$(DeployPublishDir)' == ''">$(PublishDir)</DeployPublishDir>
</PropertyGroup>

<Target Name="RunDeployAfterVSWebPublish" AfterTargets="AfterPublish">
  <!-- 打印调试信息（可选） -->
  <Message Text="[DEBUG] 使用参数: User=$(DeployUser), Host=$(DeployHost), Dir=$(DeployAppDir), Service=$(DeployServiceName), PublishDir=$(DeployPublishDir)" Importance="high" />
  
  <!-- 执行部署脚本 -->
  <Exec Command="deploy.bat $(DeployUser) $(DeployHost) $(DeployAppDir) $(DeployServiceName) $(DeployPublishDir)" />
</Target>
```



```sh
dotnet publish -c Release /p:DeployUser=admin /p:DeployHost=192.168.1.100



dotnet publish -c Release ^
  /p:DeployUser=admin ^
  /p:DeployHost=192.168.1.100 ^
  /p:DeployAppDir=/service/prod ^
  /p:DeployServiceName=MyAppProd.service ^
  /p:DeployPublishDir=bin\CustomPublish
  
  
  
  #在 .csproj 和命令行中，DeployPublishDir 既可以是 绝对路径 也可以是 相对路径
  dotnet publish /p:DeployPublishDir="bin\Release\net8.0\publish"
  #如果路径包含空格或特殊字符，需用引号包裹：
  dotnet publish /p:DeployPublishDir="C:\MyProject\bin\Release\net8.0\publish"
```



### 项目名称变量

```xml
<PropertyGroup>
  <!-- 自动使用项目名称 -->
  <ProjectName>$(MSBuildProjectName)</ProjectName>

  <!-- 定义默认参数 -->
  <DeployUser>root</DeployUser>
  <DeployHost>115.190.13.222</DeployHost>
  <DeployAppDir>/service/$(ProjectName)</DeployAppDir>
  <DeployServiceName>$(ProjectName).service</DeployServiceName>
  <DeployPublishDir>bin/Release/net8.0/publish</DeployPublishDir>
</PropertyGroup>

<Target Name="RunDeployAfterVSWebPublish" AfterTargets="AfterPublish">
  <Exec Command="deploy.sh $(DeployUser) $(DeployHost) $(DeployAppDir) $(DeployServiceName) $(DeployPublishDir)" />
</Target>
```





## 变量说明

写在生成前后事件里面的东西 可能也可以写在.csproject

例如$(SolutionDir)deploy.bat

```sh
指令说明 
$(ConfigurationName) 
当前项目配置的名称（例如，“Debug|Any CPU”）。 
$(OutDir) 输出文件目录的路径，相对于项目目录。这解析为“输出目录”属性的值。它包括尾部的反斜杠“\”。 
$(DevEnvDir) Visual Studio 的安装目录（定义为驱动器 + 路径）；包括尾部的反斜杠“\”。 
$(PlatformName) 当前目标平台的名称。例如“AnyCPU”。 
$(ProjectDir) 项目的目录（定义为驱动器 + 路径）；包括尾部的反斜杠“\”。 
$(ProjectPath) 项目的绝对路径名（定义为驱动器 + 路径 + 基本名称 + 文件扩展名）。 
$(ProjectName) 项目的基本名称。 
$(ProjectFileName) 项目的文件名（定义为基本名称 + 文件扩展名）。 
$(ProjectExt) 项目的文件扩展名。它在文件扩展名的前面包括“.”。 
$(SolutionDir) 解决方案的目录（定义为驱动器 + 路径）；包括尾部的反斜杠“\”。 
$(SolutionPath) 解决方案的绝对路径名（定义为驱动器 + 路径 + 基本名称 + 文件扩展名）。 
$(SolutionName) 解决方案的基本名称。 
$(SolutionFileName) 解决方案的文件名（定义为基本名称 + 文件扩展名）。 
$(SolutionExt) 解决方案的文件扩展名。它在文件扩展名的前面包括“.”。 
$(TargetDir) 生成的主输出文件的目录（定义为驱动器 + 路径）。它包括尾部的反斜杠“\”。 
$(TargetPath) 生成的主输出文件的绝对路径名（定义为驱动器 + 路径 + 基本名称 + 文件扩展名）。 
$(TargetName) 生成的主输出文件的基本名称。 
$(TargetFileName) 生成的主输出文件的文件名（定义为基本名称 + 文件扩展名）。 
$(TargetExt) 生成的主输出文件的文件扩展名。
```

## 常用 MSBuild 内置变量（Properties）

| 变量名                          | 描述                                                      |
| ------------------------------- | --------------------------------------------------------- |
| `$(MSBuildProjectName)`         | 项目名称（不包含 `.csproj` 后缀）                         |
| `$(MSBuildProjectFile)`         | 项目文件名（含 `.csproj` 后缀）                           |
| `$(MSBuildProjectFullPath)`     | 项目完整路径（含文件名）                                  |
| `$(MSBuildProjectDirectory)`    | 项目所在目录                                              |
| `$(MSBuildThisFile)`            | 当前被处理的 `.targets` 或 `.props` 文件的文件名          |
| `$(MSBuildThisFileDirectory)`   | 当前被处理的 `.targets` 或 `.props` 文件所在目录          |
| `$(MSBuildToolsPath)`           | MSBuild 可执行工具的路径                                  |
| `$(MSBuildExtensionsPath)`      | MSBuild 扩展的根路径                                      |
| `$(Configuration)`              | 编译配置（如 Debug / Release）                            |
| `$(Platform)`                   | 编译平台（如 AnyCPU / x64 / x86）                         |
| `$(OutputPath)`                 | 编译输出路径（相对于项目目录）                            |
| `$(TargetFramework)`            | 当前目标框架（如 net8.0）                                 |
| `$(TargetFrameworkIdentifier)`  | 框架标识符（如 `.NETCoreApp`）                            |
| `$(TargetFrameworkVersion)`     | 框架版本号（如 `v8.0`）                                   |
| `$(IntermediateOutputPath)`     | 中间输出目录（如 `obj\Debug\net8.0\`）                    |
| `$(AssemblyName)`               | 最终生成的程序集名称（不含 `.dll`）                       |
| `$(OutputType)`                 | 输出类型（如 `Exe`, `Library`, `WinExe`）                 |
| `$(RootNamespace)`              | 根命名空间                                                |
| `$(ProjectGuid)`                | 项目 GUID                                                 |
| `$(SolutionDir)`                | 解决方案目录路径（需在 `.sln` 中构建才有值）              |
| `$(SolutionName)`               | 解决方案名称（不含 `.sln`）                               |
| `$(SolutionFileName)`           | 解决方案文件名                                            |
| `$(BaseIntermediateOutputPath)` | `obj/` 目录的根路径                                       |
| `$(PublishDir)`                 | 发布目录（默认是 `bin\$(Configuration)\netX.X\publish\`） |

### 自定义变量

你也可以在 `.csproj` 中自定义变量：

```xml
<PropertyGroup>
  <DeployUser>root</DeployUser>
  <MyCustomPath>/my/path</MyCustomPath>
</PropertyGroup>
```

然后使用：

```
<Exec Command="echo $(MyCustomPath)" />
```

------

### 如何查看变量值？

你可以添加以下内容来打印变量：

```xml
<Target Name="ShowVariables" AfterTargets="Build">
  <Message Text="ProjectName: $(MSBuildProjectName)" Importance="high" />
  <Message Text="OutputPath: $(OutputPath)" Importance="high" />
</Target>
```

