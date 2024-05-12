[![Run App Boggle](https://github.com/Kashish-Syed/Boggle/actions/workflows/run-app.yml/badge.svg)](https://github.com/Kashish-Syed/Boggle/actions/workflows/run-app.yml)

# Boggle

## Table of Contents
* [Overview](#overview)
	* [Webpage](#webpage)
* [Getting Started](#getting-started)
	* [Prerequisites](#prerequisites)
	* [Installation](#installation)
* [Backend](#backend)
* [Frontend](#frontend)
* [Server (Optional)](#server-optional)
* [Usage](#usage)
* [Testing](#testing)
* [Acknowledgements](#acknowledgements)

## Overview
A word game where each player searches the assortment of letters for words of three letters or more. 
When a player finds a word, they write it down. This game can be played in three ways: single-player 
with or without a timer, and multiplayer with up to eight players.

### Webpage

<img src="./resources/webpage.png" alt="drawing" width="400"/>

## Getting Started
Start by getting the prerequisites and cloning the repo:

### Prerequisites
* .NET 8.0
* Node.js

### Installation
1. Clone the repo
	``` sh
	git clone https://github.com/Kashish-Syed/Boggle
	```

## Backend
The backend was implement in C# and .NET 8.0 and it's made up of the following class libraries:
* `/src/BoggleAccessors`: Class library for accessing the database.
* `/src/BoggleAPI`: Class library for controllers with API endpoints and the TCP server.
* `/src/BoggleContracts`: Class library for storing the class interfaces.
* `/src/BoggleEngines`: Class library for additional app logic.

The project uses the following NuGet packages:
* NUnit, Version: 4.1.0
* NUnit Analyzers, Version: 4.1.0
* NUnit3TestAdapter, Version: 4.5.0
* System.Data.SqlClient, Version: 4.8.6
* coverlet.collector, Version: 6.0.0
* Microsoft.NET.Test.Sdk, Version: 17.8.0

To resolve missing packages run:
``` sh
dotnet add package [package-name]
```

The target file is `Boggle/src/BoggleAPI/Program.cs`.

To run the backend:

1. ``` sh
	cd Boggle
	```
2. ``` sh
	dotnet restore
	```
3. ``` sh
	cd src/BoggleAPI
	```
4. ``` sh
	dotnet run
	```

**Terminal view after dotnet run: **
<img src="./resources/terminalapi.png" alt="drawing" width="200" height="100"/>



## Frontend

## Server (Optional)

## Usage

## Testing

## Acknowledgements
This project was made by:
* Dawson McGahan: [Zepherian04](https://github.com/Zepherian04)
* Kashish Syed: [Kashish-Syed](https://github.com/Kashish-Syed)
* Loc Nguyen: [locnugwin](https://github.com/locnugwin)
* Marie Victoria Zhussupova: [Phychee](https://github.com/phychee)
* Walker Lee: [walkerlee03](https://github.com/walkerlee03)
* Zaiden De La O: [BigZ5709](https://github.com/BigZ5709)
