FROM buildpack-deps:curl
COPY ./migration.sh /migration.sh
ENTRYPOINT [ "sh","migration.sh" ,"deploys-db/migration","60"]  