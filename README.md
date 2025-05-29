# ToDo_App

A To-Do App aimed at helping you reach your goals!

Features:
- Real-time updates
- The tasks must be longer than 10 characters. Otherwise an error message is displayed
- A deadline can be defined. All tasks that are overdue will be marked in red
- The tasks are displayed in a table
- They can be deleted and marked as done
- The tasks are persisted in a local data storage
- Task management via priority tasks
- Sorting/filtering
- Task pagination (separation of tasks into pages so there's no overcluttering)
- A cute little animation when you complete a task and click on "Complete"

Changes:
- Added automatic conversion of all datetimes to UTC. So all DT saved to the server is in UTC format. This DT is then automatically converted to local user DateTime on the frontend.
