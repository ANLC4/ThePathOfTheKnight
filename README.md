# ThePathOfTheKnight
Knight Path Technical Take Home - My Solution

I found this C# BFS implementation here: https://www.geeksforgeeks.org/breadth-first-search-or-bfs-for-a-graph/

For the basic graph traversal algorithm I chose Breadth First Search(BFS) and using DIP I let the possibility of implementing 
any other algorithms by writing a new class that implementing the IThePathOfTheKnight interface and changing the dependency 
injection at the Startup Class. (Strategy Pattern)

I could do the same on the Database side, however it’s out of the scope of this exercise.

I also would write some unit tests and integration tests however by the time intended to the exercise and the scope 
I decide to let that out of my last commit.

To access the EndPoint to find the Knights Path: (Replace the Start and End query params to the ones you want)
https://anlc4-knight.azurewebsites.net/api/FindThePath?code=tBDGqY5r4d2hCWfuQJxpg6JlZqcsmtgDAH7M4SaBnZaOAzFuL__Zug%3D%3D&Start=1,1&End=5,1

To access the endpoint to query the Knights Path: (Replace the Id query param to the one you need query)
https://anlc4-knight.azurewebsites.net/api/GetThePath?code=5wp0QcivO3jFTA2pWap5Up7YzYJBhrlBllYaRMC2oXWRAzFuSaqapQ%3D%3D&Id=dc69fd82-2f02-4771-9c1c-ed004658ffb1


Thank you for this opportunity to share my code with you.


Andre