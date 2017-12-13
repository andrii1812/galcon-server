#! /usr/bin/sh

cd ../galcon

unzip out.zip

if [ -d "linux-arm" ]; then
  rm -rf linux-arm
fi

if [ -d "linux-x64" ]; then
  rm -rf linux-x64
fi

if [ -d "win7-x64" ]; then
  rm -rf win7-x64
fi

mv publish/* . && rmdir publish
zip win7-x64.zip win7-x64/*
cp ../galcon-deploy/Dockerfile linux-arm/
docker build -t galcon-server linux-arm/
docker rm -f galcon-server
docker run -d --restart=always -p 5050:5050 --name=galcon-server galcon-server


