FROM node:20-alpine AS build
WORKDIR /app
# Copy only frontend package files and install
COPY FrontBooksReviews/package*.json ./
RUN npm ci
# Copy frontend source
COPY FrontBooksReviews/ ./
RUN npm run build --silent

FROM nginx:stable-alpine AS runtime
# Copy built files (handle cases where Angular outputs to dist/<project>)
RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /app/dist /usr/share/nginx/html
COPY --from=build /app/dist/* /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
