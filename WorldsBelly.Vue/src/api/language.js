import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";


function fetchLanguages() {
	return axios
		.get(domain + `/api/Languages`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}


export default {
    fetchLanguages
}
