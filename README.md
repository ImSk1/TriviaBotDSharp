# TriviaBot 🎮

TriviaBot is a fun and engaging trivia game bot for Discord, designed to challenge you and your friends with various trivia questions. Built using the DSharpPlus .NET library and connected with EFCore to a SQL Server database.

![alt text](https://cdn.discordapp.com/attachments/766732110463107105/1139534058003648622/questionScreenshot.png)

## Features

- **Multiple Categories**
- **Timed Questions**: Answer within the 15s time to score points.
- **Profile Stats**: Show off your stats to your friends.

## Installation

1. Clone the repository
2. Set up a SQL Server database
3. Modify the connection string in `appsettings.json`
4. Build and run the project

## How to Play

1. Use `!trivia` to answer a random question.
2. Use `!profile` to see your stats.
3. Answer the questions within the 15s time limit and score points.


## Legacy Disclaimer :warning:

Due to recent changes in Discord's slash command functionality, this bot is considered legacy and does not work anymore. This project is maintained here for archival purposes and educational insight into building a bot with DSharpPlus and EFCore.

## Trivia Source: OpenTDB

TriviaBot utilizes [OpenTDB](https://opentdb.com/), a free and open-source database containing thousands of public trivia questions. OpenTDB offers a variety of question categories and difficulties, making it an ideal source for diverse and engaging gameplay.

By leveraging OpenTDB, TriviaBot ensures a fresh and challenging experience every time you play. Enjoy questions from various domains such as History, Science, Art, and more!

[Learn more about OpenTDB](https://opentdb.com/api_config.php) or explore their API for your own projects.

## Contributing

Feel free to fork the project, submit a pull request, or create an issue if you find a bug or have any suggestions.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
