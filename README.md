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

# Storage
Project currently only supports uploading to DynamoDB
<img width="1425" height="763" alt="image" src="https://github.com/user-attachments/assets/539ef9b0-146a-418d-9cc0-875e0b5b26b6" />
<img width="1385" height="593" alt="image" src="https://github.com/user-attachments/assets/7ae91831-070f-4843-9ded-3b5b3d774497" />

