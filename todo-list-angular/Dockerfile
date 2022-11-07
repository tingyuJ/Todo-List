### STAGE 1: Build ###
FROM node:18-alpine AS build
WORKDIR /usr/src/app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build

### STAGE 2: Run ###
FROM nginx:alpine
COPY --from=build /usr/src/app/dist/todo-list-angular /usr/share/nginx/html
EXPOSE 80

# docker run -p 4200:80 <image_name>