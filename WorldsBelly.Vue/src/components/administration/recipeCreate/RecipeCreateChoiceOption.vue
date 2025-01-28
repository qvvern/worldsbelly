<template>
  <div class="option-card" :class="{'option-card-selected' : selected, 'option-card-yellow' : yellow, 'option-card-orange' : orange}">
      <div class="option-card__icon">
        <v-icon>{{ icon }}</v-icon>
      </div>
      <div class="option-card__title">
        {{ name }}
      </div>
      <div class="option-card__subtitle pb-1">
        {{ description | maxLength(65) }}
      </div>
      <!-- <div class="option-card__select">
        <v-icon>icofont-thin-right</v-icon>
      </div> -->
  </div>
</template>

<script>
    export default {
        name: "RecipeCreateChoiceOption",
        props: {
          name: String,
          namePlural: String,
          description: String,
          icon: String,
          selected: Boolean,
          yellow: Boolean,
          orange: Boolean,
        },
    };
</script>

<style lang="scss" scoped>
.option-card {
  min-height: 120px;
  text-align: center;
  // border: 2px solid #524e4e;
  border: 2px solid #fff;
  border-radius: 10px;
  width: 220px;
  // transition: all 500ms;
  background: #ffffff;
  cursor: pointer;
  -webkit-box-shadow: 0 0 10px rgba(0,0,0,0.2);
  -moz-box-shadow: 0 0 10px rgba(0,0,0,0.2);
  box-shadow: 0 0 10px rgba(0,0,0,0.2);
  &-selected {
    transition: all 300ms;
    background: var(--highlight);
    // border-color: darken($highlight, 4%);
    * {
      color: #fff !important;
    }
  }
  &-yellow {
    background: var(--yellow);
    border-color: #fff;
    * {
      color: #fff !important;
      text-shadow: 1px 1px 10px rgba(150, 150, 150, .5);
    }
  }
  &-orange {
    background: #fdbd10;
    border-color: #d89a15;
    transition: all 300ms;
    // width: 205px;
    -webkit-box-shadow: 0px 0px 15px -4px rgba(0,0,0,0.9);
    -moz-box-shadow: 0px 0px 15px -4px rgba(0,0,0,0.9);
    box-shadow: 0px 0px 15px -4px rgba(0,0,0,0.9) !important;
    .v-icon {
      opacity: 1;
      color: #fff !important;
    }
  }
  &:hover {
    background: var(--highlight);
    border-color:  var(--highlight);
    box-shadow: 0 0 30px rgba(0,0,0,0.35);
    * {
      color: #fff !important;
    }
  }
  &__icon {
    padding-top: 15px;
    .v-icon {
      font-size: 3em;
      font-weight: 300;
      opacity: .3;
      color: #000;
    }
  }
  &__title {
    word-break: break-word;
    padding-top: 4px;
    visibility: visible;
    list-style: none;
    box-sizing: border-box;
    text-transform: unset;
    font-size: 18px;
    font-weight: 700;
    font-stretch: normal;
    font-style: normal;
    line-height: normal;
    letter-spacing: 0.07px;
    text-align: center;
    color: #1d1d1f;
    position: relative;
    margin: auto;
  }
  &__subtitle {
      list-style: none;
      box-sizing: inherit;
      margin: 0;
      padding: 0;
      border: 0;
      vertical-align: baseline;
      color: #8a8793;
      letter-spacing: .2px;
      font-size: 12px;
  }
  &__select {
    padding: 2px;
    .v-icon {
      font-size: 1em;
      color: #000;
    }
  }
}
</style>
