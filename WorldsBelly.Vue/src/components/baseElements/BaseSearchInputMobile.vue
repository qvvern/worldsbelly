<template>
      <div class="search-wrapper-mobile">
        <svg
          :style="{color: isHome ? '#fff' : '#000'}"
          @click="handleClick"
          class="cursor-pointer feather feather-search"
          xmlns="http://www.w3.org/2000/svg"
          width="25"
          height="25"
          fill="none"
          stroke="currentColor"
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          viewBox="0 0 24 24">
          <defs></defs>
          <circle cx="11" cy="11" r="8"></circle>
          <path d="M21 21l-4.35-4.35"></path>
        </svg>
      </div>
</template>


<script>
  export default {
      name: "BaseSearchInputMobile",
      props: {
        isHome: Boolean,
      },
      methods: {
        handleClick() {
          this.$emit('onClick');
        }
      }
  };
</script>
<style lang="scss">

.search-wrapper-mobile {

}

</style>
