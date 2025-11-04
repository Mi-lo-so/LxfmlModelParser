# ModelParserWebApp

ModelParserWebApp is a ASP.NET Core Web API that parses LXFML LEGO model files and stores the parsed model
items in DynamoDB.

This README explains how to run and interact with the service locally.

The application initializes services from the ModelParserApp library and checks if the dynamo
table exists at start up, and initializes it with expected partition and sort key if it does not.

## Features

This web app exposes a minimal HTTP API for:

- Uploading an LXFML file to be parsed and stored in DynamoDB by file name and total brick count.
- Reading a model item from DynamoDB by id by the GET endpoint.

## Prerequisites

- You must be logged into an AWS account. This project is set up in `AwsServiceModule.cs` to use an account called
  `personal`, but this can be changed.
- You can define a `personal` account in your `~/.aws/configure` with `aws configure sso --profile personal`, and set
  the AWS secret key and access key.
- Then log in with `aws sso login --profile personal`
- The AWS account must have rights to create/read/write to DynamoDB.
- Run the web api, and call the `upload` endpoint locally with the file path:

## Usage

Run the web app from the |> in your IDE, or with `dotnet run` from the directory.

Run the 'upload' endpoint to upload a file (ensure a correct file path - this one uses the example file included in the
project)

```bash
curl -F "file=@/.../ModelParserApp/ModelParserApp/ExampleFiles/everyoneisawesome.lxfml" http://localhost:5253/api/lxfml/upload
```

- Run the 'read' for a model name endpoint with:

```bash
  curl http://localhost:5253/api/lxfml/everyoneisawesome.lxfml
```

or with prettier json print output:

```bash
curl -s http://localhost:5253/api/lxfml/a0081332-03c8-45a2-9bd4-f11839c81511 | jq
```

## Setup

Ensure that the table name is defined in `appSettings.json` and/or `appSettings.Development.json` like:

```bash
"ModelDynamoDbStorage": {
    "TableName": "LxfmlModels"
  }
```

## Architecture

The `Program.cs` initializes the services from the ModelParserApp library,
and initializes a DynamoDB table for the current AWS account on start up.

The `LxfmlController.cs` defines API endpoints to:

- create an entity in DynamoDB, by uploading LXFML files for parsing.
- read stored entities in DynamoDB

### LXFML file

### TODO

