import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";


function fetchIngredients(filter) {
	return axios
		.get(domain + `/api/Ingredients?startAt=${filter.startAt}&amount=${filter.amount}&search=${filter.search}`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function fetchIngredientsById(ids) {
	return axios
		.post(domain + `/api/Ingredients/id`, ids)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

export default {
    fetchIngredients,
    fetchIngredientsById
}
