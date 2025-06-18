## To Do App Demo image:

![To-DoApp_Demo](https://github.com/user-attachments/assets/795a80dd-e053-4a4f-a661-903b890599a3)

## Features
1. The tasks must be longer than 10 characters. Otherwise an error message is displayed.
2. A deadline can be defined. All tasks that are overdue will be marked in red.
3. They can be deleted and marked as done.
4. The tasks are persisted in a data storage.
5. Real-time updates (1 min refresh)
6. Task management via priority tasks (Priority tasks marked with the pin icon will be pinned to the top of the task list)
7. Task pagination (separation of tasks into pages so there's no overcluttering)
8. Sorting/filtering
9. Tasks are displayed on a table that can be sorted according to deadline, alphabetical order or creation date
10. A cute little animation when you complete a task and click on "Complete"

## Changes:
- Added automatic conversion of all datetimes to UTC. So all DT saved to the server is in UTC format. This DT is then automatically converted to local user DateTime on the frontend.
- Added logging functionality
