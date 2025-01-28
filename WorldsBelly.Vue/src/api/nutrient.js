import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";


function fetchNutrients() {
	return axios
		.get(domain + `/api/Nutrients`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}


export default {
    fetchNutrients
}
