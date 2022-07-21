global_rematch() { 
    local s=$1 regex=$2 
    while [[ $s =~ $regex ]]; do 
        echo "${BASH_REMATCH[1]}"
        s=${s#*"${BASH_REMATCH[1]}"}
    done
}

ticket_url="https://andreani.atlassian.net"
ticket_auth="Authorization: Basic YWtvd2FsY3p1a0BhbmRyZWFuaS5jb206WThSTkxIbTM0N3k0N1hxbkdTc1Q3OTBD"

printf "\n"
echo "COMMENT SENDER JOB START"
printf "\n"

repo_url=$(git remote get-url origin)

commit_url=""
if [[ "$repo_url" == "https://"* ]]
then
 commit_url="${repo_url::-4}"
elif [[ "$repo_url" == "git@"* ]]
then 
 commit_url="https://github.com/${repo_url:15:-4}"
else
 commit_url=""
fi

echo Repo URL: "${commit_url}"

log=$(git log --max-count=1 --format=%B | tr '[a-z]' '[A-Z]')
commit_hash=$(git rev-parse HEAD~0)

echo "Push Log: ${log}"

echo "Processing log to get ticket references..."

regex="(#+[^[:space:]]+)"

mapfile -t matches < <( global_rematch "$log" "$regex"  )

printf "\n"
echo "Reference found [${#matches[@]}]"

for issue in ${matches[@]}
do

	get_ticket_path="${ticket_url}/rest/api/2/issue/${issue:1}"
	post_ticket_path="${get_ticket_path}/comment"
	printf "\n"
    echo "---------------------------------------------------------"
    echo "Ticket: ${issue}"
	
	printf "\n"

	request_cmd="$(curl --location --request GET "${get_ticket_path}" -i -o - --silent --header 'Accept: application/json' --header 'Content-Type: application/json' --header "${ticket_auth}")"
	
	http_status=$(echo "$request_cmd" | grep HTTP |  awk '{print $2}')

	if [[ "$http_status" -ne 200 ]]
	then
		echo "[${issue}] was not found or you don't have access."
	else
		summary_regex='("summary":"[^"]+)'
		mapfile -t summary_matches < <( global_rematch "$request_cmd" "$summary_regex"  )
		
		if [[ ${#summary_matches[@]} -gt 0 ]]
		then
			echo "Description: ${summary_matches[1]:11}"
		else
			echo "Description: unknown"
		fi
	
		printf "\n"
		
		echo "Posting comment into ticket services..."
		
		message="h3. Git notification service.\n----\n  \nCommit tracker info: [${commit_url}/commit/${commit_hash}] \n\n\n\n*Message*\n\n{quote}$log{quote}\n\n----"
		
		printf "\n----Git Message---- \n $message \n-------------------\n"
		
		#posting_cmd="$(curl --location --request POST "${post_ticket_path}" -i -o - --verbose --header 'Accept: application/json' --header 'Content-Type: application/json' --header "${repoauth}" --data-raw '{"body": "${message}"}' --header 'Cookie: atlassian.xsrf.token=BU2Z-CPDC-QFRP-1D36_e7706b098004252a1a7f54f5e336d4cb0add7167_lin')"
		body_message="{ \"body\": \"${message}\" }"
		echo "${body_message}"
		
		posting_cmd="$(curl -i --location --request POST "${post_ticket_path}" --header 'Accept: application/json' --header 'Content-Type: application/json' --header 'Authorization: Basic YWtvd2FsY3p1a0BhbmRyZWFuaS5jb206WThSTkxIbTM0N3k0N1hxbkdTc1Q3OTBD' --data-raw "{\"body\":\"$message\"}")"
		
		posting_cmd_http_status=$(echo "$posting_cmd" | grep HTTP |  awk '{print $2}')
		echo "${posting_cmd_http_status}"
		if [[ "$posting_cmd_http_status" -eq 200 ]]
		then
			echo "comment has done."
		else
			echo "Unable to create a comment."
			echo "Request result: ${posting_cmd}"
		fi
	fi
	
    echo "---------------------------------------------------------"
done

printf "\n"
echo "Issue comment sender job has end."


