# Jobsity Challenge

## Description
This project is designed to test your knowledge of back-end web technologies, specifically in
.NET and assess your ability to create back-end products with attention to details, standards,
and reusability.


## Assignment
The goal of this exercise is to create a simple browser-based chat application using .NET.
This application should allow several users to talk in a chatroom and also to get stock quotes
from an API using a specific command.

## Solution
This solution uses signalR to create a functional chatroom and is divided in 3 projects:

- JobsityFinancialChat: The web app frontend.
- JobsityStockBot: The bot in charge of querying the stock data.
- JobsityCommon: A project to group all that is in common of the other projects.

## Requirements to run this solution
Docker must be isntalled in the on your system.


[Install Docker Desktop on Linux](https://docs.docker.com/desktop/linux/install/)

[Install Docker Desktop on Mac](https://docs.docker.com/desktop/mac/install/)

[Install Docker Desktop on Windows](https://docs.docker.com/desktop/windows/install/)

#How to run the App

- Clone this repository into your system.
- Navigate to the root folder.
- Open a terminal in this path.
- Run the command `docker-compose up --build`
- After the command ends, go to your browser and access http://localhost:7000

# Features included

- Messages are ordered by their timestamps.
- You can use the command `/stock=stock_code` to retrieve information about the stock.
- All the messages from the users are stores in a Sql Server database (Excluded the bot messages).
- Limit of 50 messages per chat.
- .NET identity authentication.
- Unit test of bot commands.





