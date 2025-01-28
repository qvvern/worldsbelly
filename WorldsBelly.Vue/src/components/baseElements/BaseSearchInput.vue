<template>
      <div class="search-wrapper">
        <input
          ref="search"
          :value="value"
          @input="handleInput"
          class="search-wrapper-input"
          type="text"
          :placeholder="placeholder"
          v-on:keyup.enter="handleEnter">
        <svg
          @click="handleClick"
          class="cursor-pointer feather feather-search"
          xmlns="http://www.w3.org/2000/svg"
          width="20"
          height="20"
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
      name: "BaseSearchInput",
      props: {
        value: [String, Number],
        placeholder: String
      },
      methods: {
        handleInput(event) {
          this.$emit('input', event.target.value);
        },
        handleEnter(event) {
          this.$emit('enter', event.target.value);
        },
        handleClick() {
          this.$emit('input', this.$refs.search.value);
        }
      }
  };
</script>
<style lang="scss">

.search-wrapper {
  border: 1px solid #e0e0e0;
  border-radius: 20px;
  background-color: #fff;
  padding-right: 12px;
  height: 40px !important;
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
  max-width: 480px;
  color: #000;
  box-shadow: 0 2px 6px 0 rgba(136,148,171,.2),0 24px 20px -24px rgba(71,82,107,.1);
  overflow: hidden;

}

.search-wrapper-input {
  border: none;
  flex: 1;
  outline: none;
  height: 100%;
  padding: 0 20px;
  font-size: 14px;
  background-color: #fff;
  color: #1f1c2e;

  &:placeholder {
    color: #1f1c2e;
    opacity: .6;
  }
}

</style>
