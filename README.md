# MovieApi
Sample Movie Api

--Initial--
This projecct would require .NET6
Once downloaded, please set the MovieApi project as the startup project and run.
Upon running you will the see the Swagger UI with the endpoints
/api/Movie and /WeatherForecast are just test endpoints and it should smoothly work.

--Authorization--
Now if you try to run the /api/Movie/search, it should throw 401 Unauthorized as the endpoints are protected.
We have used token based authentication and authorization mechanism from Auth0 which acts as the identity server.
There are some predefined users as mentioned below:
movieuser1@auth.com (password: MovieUser1) (permission: read, role: moviewatcher) - can search for a movie, see top movies, top movies by user and can rate the movie and add movies to their collection 
movieuser2@auth.com (password: MovieUser2) (permission: read, role: moviewatcher) - can search for a movie, see top movies, top movies by user and can rate the movie and add movies to their collection 
movieadmin1@auth.com (password: MovieAdmin1) (permission: add, edit, role: moviewatcher) - can add new movie or update the existing
moviesuperadmin1@auth.com (password: MovieSuperAdmin1) (permission: delete, role: moviewatcher) - can delete the movie
To get authorized to the endpoints, please click on Authorize button or lock icon on the Swagger UI. It will ask you to login into Auth0. You can use any of the above user email and login. You will be redirected to the webpage.
If that webpages says error, then please let me know, it is possible that redireccturl is not added for the port on which the Swagger UI is running on your browser. I will add it.
If it directly works, then you will be able to see the message popup which authorization successful. You can close the popup and see yourself authorized to run the api.
Please see the users and the permissions mentioned above carefully so that you can access the endpoints appropriately.

--Data--
To make it simple we are using json file to fetch and save all the data. When there is an add/update record action you can go to the json files and see the updated records over there. Dont forget to format the document upon right click for better visibility.

In the source code you can find below under root:
movies.json - stores movie details
users.json - stores list of resgitered users with Auth0 as mentioned above
usersRating - rating done by any user for a particular
usersMoviesCollection - collection of movies for any user 

--Endpoints--



