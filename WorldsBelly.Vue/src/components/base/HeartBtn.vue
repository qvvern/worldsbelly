<template>
  <button :class="['heart-btn', {liked: this.value}]" :style="{'pointer-events': isUpdating ? 'none' : 'auto'}" @click="heartit" :disabled="!$msal.isAuthenticated">
  <svg class="heart heart-icon" viewBox="0 0 32 29.6">
    <path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2
    c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/>
  </svg>
</button>
</template>
<script>
import { mapActions } from "vuex";
export default {
  name: "HeartBtn",
    props: {
        recipeId: Number,
        value: Boolean
    },
    data() {
      return {
        isUpdating: false
      }
    },
 methods: {
  ...mapActions('recipe/', ['like_recipe']),
  heartit: function (e) {
    this.isUpdating = true;
   const hearts = document.createElement("div");
   hearts.innerHTML =
    '<svg class="heart heart-pop one" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg><svg class="heart heart-pop two" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg><svg class="heart heart-pop three" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg><svg class="heart heart-pop four" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg><svg class="heart heart-pop five" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg><svg class="heart heart-pop six" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg><svg class="heart heart-pop seven" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg><svg class="heart heart-pop eight" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg><svg class="heart heart-pop nine" viewBox="0 0 32 29.6"><path d="M23.6,0c-3.4,0-6.3,2.7-7.6,5.6C14.7,2.7,11.8,0,8.4,0C3.8,0,0,3.8,0,8.4c0,9.4,9.5,11.9,16,21.2c6.1-9.3,16-12.1,16-21.2C32,3.8,28.2,0,23.6,0z"/></svg>';
   e.target.appendChild(hearts);
  this.like_recipe(this.recipeId).finally(() => {
    setTimeout(() => {
    this.isUpdating = false;
    }, 500);
  });
   this.$emit('input', !this.value);
   setTimeout(function () {
    e.target.removeChild(hearts);
   }, 3000);
  }
 },
 mounted() {
  document.body.addEventListener("mousedown", function () {
   document.body.classList.add("using-mouse");
  });
  document.body.addEventListener("keydown", function () {
   document.body.classList.remove("using-mouse");
  });
},
}
</script>

<style lang="scss">

.heart{
  width: 20px;
  fill: gray;
  transition: fill .5s, transform .5s;
  pointer-events: none;

  &-btn{
    position: relative;
    background: transparent;
    border-radius: 50%;
    // background-color: white;
    padding: 2px 1px;
    // box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.25), 0px 2px 5px 0px rgba(0, 0, 0, 0.2);
    border: 0;

    &.is-updating {
      pointer-events: none;
    }
    &:hover{
      .heart {
        fill: #ee3529;
      }
    }
    &.liked{
      .heart{
        fill: #ee3529;

       &-icon{
        transform: scale(1.2);
       }

        &-pop{
          animation-direction: normal;
        }
      }
    }
  }

  &-pop{
   position: absolute;
   width: 30%;
   opacity: 0;
   left: 0;
   animation: pop 1s ease forwards reverse;

    &.one{
      left: 1%;
      transform: rotate(-10deg);
      animation-delay: .45;
    }
    &.two{
      left: 15%;
      transform: rotate(8deg);
      animation-delay: .25s;
    }
    &.three{
      left: 30%;
      transform: rotate(-5deg);
      animation-delay: 0;
    }
    &.four{
      left: 50%;
      transform: rotate(-8deg);
      animation-delay: .3s;
    }
    &.five{
      left: 60%;
      transform: rotate(-10deg);
      animation-delay: .9s;
    }
    &.six{
      left: 70%;
      transform: rotate(-8deg);
      animation-delay: .2;
    }
    &.seven{
      left: 85%;
      transform: rotate(6deg);
      animation-delay: .35s;
    }
    &.eight{
      left: 90%;
      transform: rotate(-4deg);
      animation-delay: .5s;
    }
    &.nine{
      left: 95%;
      transform: rotate(8deg);
      animation-delay: .2s;
    }
  }
}

@keyframes pop{
  0%{
    left: calc(50% - 7px);
    top: 10px;
  }
  20%{
    opacity: 1;
  }
  80%{
    top: -20px;
  }
  100%{
    opacity: 0;
  }
}
</style>
