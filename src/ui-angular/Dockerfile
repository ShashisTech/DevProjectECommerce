# Stage 1: build
FROM node:18 AS build
WORKDIR /app
COPY src/ui-angular/package*.json ./
RUN npm install
COPY src/ui-angular/ ./
RUN npm run build -- --outputPath=./dist

# Stage 2: serve
FROM nginx:stable-alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
