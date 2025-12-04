#!/bin/bash
# Used for local container development to build images and start and remove containers

# Arg validation
valid_args=',build_and_run,build,run,rm'
if [[ $# -gt 1 || !("$valid_args" =~ (","|^)$1(","|$)) ]];
then
	echo 'invalid args'
	exit
fi

if [[ $# -eq 0 || "$1" = 'build' || "$1" = 'build_and_run' ]];
then
	# Buildah builds images
	buildah bud -t keycloak-image Dockerfile.webapp.dev
	# Display images
	sleep 1
	buildah images | grep keycloak-*
fi

# Start containers
if [[ $# -eq 0 || "$1" = 'run' || "$1" = 'build_and_run' ]];
then
	# Create pod
	podman pod create --name keycloak-pod -p 8080:8080
	# Create container
	podman run -d --env-file=.env -v $(pwd)/deploy/local/realms:/opt/keycloak/data/import --name=keycloak-webapp --pod=keycloak-pod keycloak-image
	# Display containers
	podman ps -a
	echo -e '\nKeycloak running at http://localhost:8080. Database setup completes in 10 seconds.\n'
	
fi

# Remove containers
if [[ "$1" = 'rm' ]];
then
	# remove pod and containers
	podman pod rm -f keycloak-pod
	# Display remaining containers (if any)
	podman ps -a
fi
