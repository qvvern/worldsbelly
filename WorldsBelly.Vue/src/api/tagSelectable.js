import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";



function fetchTagSelectableChoices() {
	return axios
		.get(domain + `/api/TagSelectables/ChoiceOrders`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}


export default {
  fetchTagSelectableChoices
}
