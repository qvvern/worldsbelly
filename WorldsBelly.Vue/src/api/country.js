import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";


function fetchCountries() {
	return axios
		.get(domain + `/api/Countries`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}


export default {
    fetchCountries
}
