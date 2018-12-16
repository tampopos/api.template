#!/bin/bash

if [ $# -ne 2 ]; then
  exit 1
fi
url=$1
limit=$2
time=0

while :
do
  result=$(curl -X POST $url -H "Content-Length:0")
  if [ "x$result" = 'x0' ]; then
    echo '成功'
    exit 0
  fi
  if [ $time -ge $limit ]; then
    echo '失敗'
    exit 1
  fi
  echo 'wait 5 sec...'
  sleep 5
  let time=time+5
done
