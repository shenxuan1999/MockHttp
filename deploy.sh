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