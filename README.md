# Todo-List
This project is build with **Angular** + **.Net Web API** + **MongoDB**.

## About
I build this app to learn <strong>MongoDB</strong> and <strong>NUnit</strong>. <br>
And to practice a few other things such as <strong>JWT(Json Web Token)</strong> and <strong>Docker</strong>. <br>

## Try it out!!
There are 3 ways to run it locally with docker.
1. Run by themselves. 
2. With docker compose.
3. Clone the repo.

### 1. Run them seperately by themselves
Run the following commands:  
```
docker run -d --name mongo mongo  
docker run -d -p 5000:5000 --link mongo:mongo tingyu946/todo-list-web-api  
docker run -p 4200:80 tingyu946/todo-list-angular
```
### 2. With docker compose
Copy the [docker-compose.yml](https://github.com/tingyuJ/Todo-List/blob/master/docker-compose.yml") file and run the command:  
`docker-compose -f docker-compose.yml up`
<br>
<br>

### 3. Clone the repo
Play with them!! And use `docker-compose -f docker-compose-build.yml up` to build them locally. 
<br>
<br> 

**Checkout your http://localhost:4200 to play with it after it's up and running. :thumbsup:**

<br> 

## Dockerhub :whale:
Find the images on Dockerhub    
Front-end: [tingyu946/todo-list-angular](https://hub.docker.com/repository/docker/tingyu946/todo-list-angular)  
Back-end: [tingyu946/todo-list-web-api](https://hub.docker.com/repository/docker/tingyu946/todo-list-web-api)  


