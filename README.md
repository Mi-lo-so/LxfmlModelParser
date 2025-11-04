# LxfmlModelParser
Project for parsing LXFML model files.

Reads LXFML into a model with some relevant summaries of total bricks, parts, and materials, and outputs and stores the model in a storage service (dynamoDb).

# Features
- Parses LXFML into an model in-memory, to output and store in a storage service.
- Simple .Net Controller-based API to upload and query.

# Web UI
- See the [Web app README](https://github.com/Mi-lo-so/LxfmlModelParser/blob/main/ModelParserWebApp/README.md) for how to query the API endpoints

# Library
- See the [ModelParserApp library](https://github.com/Mi-lo-so/LxfmlModelParser/blob/main/ModelParserWebApp/README.md) for the services used in parsing and uploading to storage service.

