import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";


function fetchCustomTags(filter) {
	return axios
		.get(domain + `/api/UserTags?startAt=${filter.startAt}&amount=${filter.amount}&search=${filter.search}`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}


export default {
    fetchCustomTags
}
