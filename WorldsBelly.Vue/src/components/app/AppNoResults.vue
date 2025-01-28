<template>
	<div @click="$emit('OnInteraction')">
    {{ title }}
  </div>
</template>

<script>
export default {
	name: 'appNoResults',
	props: {
    title: String,
	},
	data() {
		return {
		}
	},
}
</script>
