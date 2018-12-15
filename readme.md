# tmpps.boardless

tmpps.boardless の API サーバー

## Required

- .Net core 2.1
- node.js
- AWS SQS  
  アプリケーション設定に認証情報を追加する

#### Options

- vscode
- Docker で Database を作成する場合
  - docker
  - docker-compose
  - linux

## Usage

#### restore

`dotnet restore Api/;dotnet restore Domain.Tests/`

#### build

`dotnet build Api/`

#### test

`dotnet test Domain.Tests/`

#### run

`sudo docker-compose up`

#### debug

vscode でデバッグ実行可能

#### validate ci config

`circleci config validate`

#### release

```bash
git tag X.Y.Z
git push origin --tags
```

## リンク

- [ドキュメント](https://github.com/wakuwaku3/tmpps.boardless.docs)
