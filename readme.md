# ChatGPT API based Map

"Site to ask ChatGPT using API and show the resulting geo places on the Azure map"

## Description

This project is a web application that allows users to search for locations using the ChatGPT API and display the resulting geo places on the Azure map. It also allows users to upload files to the Azure blob. The project is deployed using Azure Static App and has both front-end and back-end components.

## Deployment

The project is currently deployed at https://jolly-ocean-0da9e6d03.2.azurestaticapps.net.

## Technologies

- Front-end: React, Bootstrap, TypeScript
- Back-end: .NET 7, EF Core, ChatGPT API, Azure Service Bus, AppInsights

## Features

- Post search requests to the ChatGPT API
- Display the latest queries
- Allow routing between conversations and selected points
- Display conversations, points, and markers on the map
- Allow file upload to the Azure blob

## Routes

The following routes are available:

- _/_ - displays the app and its features.
- _/:conversationId_ - allows the user to select a specific conversation and view its details.
- _/:conversationId/:id_- allows the user to select a specific conversation and a specific point to view its details.

## Endpoints

The following endpoints are available:

| Name      | Method | Endpoint            | Description                            |
| --------- | ------ | ------------------- | -------------------------------------- |
| Chat      | POST   | /api/chat/start     | Creates a new AI conversation.         |
| Chat      | GET    | /api/chat           | Get last conversations list.           |
| Chat      | GET    | /api/chat/{id}      | Get conversation by id.                |
| Chat      | POST   | /api/chat/{id}      | Posts new message to the conversation. |
| Locations | GET    | /api/locations      | Get some points.                       |
| Locations | GET    | /api/locations/{id} | Get locations details by id.           |
| Upload    | POST   | /api/upload         | Uploads file to the blob.              |

## Queues

The following queues are available:

- file_upload_queue

## Services

### Upload File Service

A RabbitMQ worker to process file_upload_queue. (do nothing)
