# MovieApi
Movie Api is a REST API with multiple endpoints to interact with movie database

--Technology--
.NET 6 - ASP.NET Core Web Api

--Initial--
Please see this readme file in edit mode for the better indentation.
This projecct would require .NET6
Once downloaded, please set the MovieApi project as the startup project and run.
Upon running you will the see the Swagger UI with the endpoints
/api/Movie and /WeatherForecast are just test endpoints and it should smoothly work.

--Authorization--
Now if you try to run the /api/Movie/search, it should throw 401 Unauthorized as the endpoints are protected.
We have used token based authentication and authorization mechanism from Auth0 which acts as the identity server.
There are some predefined users as mentioned below:
movieuser1@auth.com (password: MovieUser1) (permission: read, role: MovieWatcher) - can search for a movie, see top movies, top movies by user and can rate the movie and add movies to their collection 
movieuser2@auth.com (password: MovieUser2) (permission: read, role: MovieWatcher) - can search for a movie, see top movies, top movies by user and can rate the movie and add movies to their collection 
movieadmin1@auth.com (password: MovieAdmin1) (permission: add, edit, role: MovieAdmin) - can add new movie or update the existing
moviesuperadmin1@auth.com (password: MovieSuperAdmin1) (permission: delete, role: MovieSuperAdmin) - can delete the movie
To get authorized to the endpoints, please click on Authorize button or lock icon on the Swagger UI. It will ask you to login into Auth0. You can use any of the above user email and login. You will be redirected to the webpage.
If that webpages says error, then please let me know, it is possible that redireccturl is not added for the port on which the Swagger UI is running on your browser. I will add it.
If it directly works, then you will be able to see the message popup which authorization successful. You can close the popup and see yourself authorized to run the api.
Please see the users and the permissions mentioned above carefully so that you can access the endpoints appropriately.
To logout, please click on logout button in the popup in swagger ui or open the url in incognito window.

--Data--
To make it simple we are using json file to fetch and save all the data. When there is an add/update record action you can go to the json files and see the updated records over there. Dont forget to format the document upon right click for better visibility.

In the source code you can find below under root:
movies.json - stores movie details
users.json - stores list of resgitered users with Auth0 as mentioned above
usersRating - rating done by any user for a particular
usersMoviesCollection - collection of movies for any user 

--Endpoints--
/api/Movie - Test endpoint


/api/Movie/new - Add new movie
Requires authorization and Add/Edit permission or MovieAdmin role
Sample input json for a movie:
{
  "title": "Avatar Returns",
  "year": 2022,
  "genre": 1,
  "runTime": "180 mins",
  "poster": "https://m.media-amazon.com/images/M/MV5BMjIwMjE1Nzc4NV5BMl5BanBnXkFtZTgwNDg4OTA1NzM@._V1_SX300.jpg",
  "averageRating": 8
}


/api/Movie/search - Searches for a movie
Requires authorization and Read permission or MovieWatcher role
Sample input json for a movie:
{
  "title": "The Lion King",
  "year": 0,
  "genres": [
    0
  ]
}

/api/Movie/top - Gives top x movies based on Avrage Rating
Requires authorization and Read permission or MovieWatcher role

/api/Movie/top/{userId} - Gives top movies based on Avrage Rating for a loggedIn user
Users cannot see top movies for any other user, but only to themselves
Requires authorization and Read permission or MovieWatcher role
Sample Input: 
userId = 632f6006e322eed6dbd1b4b7

/api/Movie/rating - LoggedIn user rates a movie
Users cannot rate a movie on behalf of any other user, but only for themselves
Requires authorization and Read permission or MovieWatcher role
Sample Input:
{
  "userId": "632f6006e322eed6dbd1b4b7",
  "movieId": "m7",
  "rating": 6.9
}

/api/Movie/collection - LoggedIn user adds a movie to their collection
Users can only add/edit movies to their collection and not to any other users collection
Requires authorization and Read permission or MovieWatcher role
Sample Input:
{
  "userId": "632f6006e322eed6dbd1b4b7",
  "movieId": "m7"
}

/api/Movie/edit - Edits a movie - Not implemented

/api/Movie/delete - Deletes a movie - Not implemented

--Unit Testing--
We have tried create Unit Tests for API end points/Controller, but to due time limitation not added for Service and Repository methods.

--Expeccted Error Codes--
401 - Unauthorized - When the user is not authenticated
403 - Forbidden - When you try to execute an endpoint and dont have permission for it. Like movieuser1 user cannot access delete endpoint as it will not have that permssion. You will have to login as moviesuperadmin1 user.

--Features--
1. REST Api
2. AuthO integration for authentication and authorization for token based authentication
3. Permission based access to end points
4. Every user is able to see collections of other people, but to be able to change only their own collection(favorites)
5. API endpoints has Unit Tests
6. Global exception handling is implemented

--Improvements--
1. Edit, Delete and DeleteAll enpoints can be implemented
2. Database and DBContexts can be added
3. Unit Tests for Service and Respository can be written
4. Comments can be added to all the relevant places
5. Logger library can be integrated to save logs in other places. Right now being logged in the response stream.
6. Role Based access can be implemented, which was time consuming to do it with Auth). Hence implemented permission based access right now.

--End of file--














