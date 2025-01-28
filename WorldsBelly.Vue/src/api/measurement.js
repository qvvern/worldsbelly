import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";

function fetchMeasurements() {
	return axios
		.get(domain + `/api/Measurements`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}


export default {
    fetchMeasurements
}
