# ModelParserWebApp

## Usage

- Log into a personal AWS account -
- Run the web api, and call the `upload` endpoint locally with the file path:

```bash
curl -F "file=@/.../ModelParserApp/ModelParserApp/ExampleFiles/everyoneisawesome.lxfml" http://localhost:5253/api/lxfml/upload
```

- Run the 'read' endpoint with:

```bash
 curl http://localhost:5253/api/lxfml/a0081332-03c8-45a2-9bd4-f11839c81511
```

or in prettier json print:

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

