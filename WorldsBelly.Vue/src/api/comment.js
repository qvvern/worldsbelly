import axios from 'axios'
import { domain } from "@/utils/constants/ApiConstants.js";

function fetchComments(recipeId) {
	return axios
		.get(domain + `/api/recipes/${recipeId}/comments`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function createComment(payload) {
	return axios
		.post(domain + `/api/recipes/${payload.recipeId}/comments`, payload.comment)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

function deleteComment(id) {
	return axios
		.delete(domain + `/api/recipes/comment/${id}`)
		.then((response) => response.data)
		.catch((error) => {
			console.log(error)
			throw error
		})
}

export default {
  fetchComments,
  createComment,
  deleteComment
}
