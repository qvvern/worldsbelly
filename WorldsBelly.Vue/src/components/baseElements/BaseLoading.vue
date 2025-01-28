<template>
  <div v-if="value" :data-id="_uid" :class="classObj" class="base-loading flex flex-col w-full align-center p-4 h-full justify-center">
      <v-progress-circular
        :size="size"
        :color="color"
        indeterminate
      ></v-progress-circular>
      <div class="base-loading-indication" v-if="progressIndication">
          <span class="base-loading-text">{{ currentSentence }}</span>
          <span class="base-loading-dots">
              <span>.</span>
              <span>.</span>
              <span>.</span>
          </span>
      </div>
  </div>
</template>

<script>
    export default {
        name: "baseLoading",
        props: {
          value: {
            type: [Boolean],
            default: false
          },
          size: {
            type: [Number],
            default: 50
          },
          color: {
            type: [String],
            default: "#62ba2e"
          },
          overlay: {
            type: [Boolean],
            default: false
          },
          fade: {
            type: [Boolean],
            default: false
          },
          parent: {
            type: [Boolean],
            default: false
          },
          progressSpeed: {
              type: Number,
              default: 2500,
          },
          progressSentences: {
              type: Array,
              default: () => [],
          },
        },
        data() {
            return {
                currentSentence: '',
                element: null
            }
        },
        computed: {
          classObj() {
            return {
              'base-loading-overlay': this.overlay,
              'fade-in': this.fade
            }
          },
          progressIndication() {
              return this.progressSentences.length > 0
          },
        },
      created() {
          if (this.progressIndication) {
              let index = 1

              this.currentSentence = this.progressSentences[0]
              const changeSentence = setInterval(() => {
                  if (index < this.progressSentences.length) {
                      this.currentSentence = this.progressSentences[index]
                      index += 1
                  } else {
                      clearInterval(changeSentence)
                  }
              }, this.progressSpeed)
          }
      },
    watch: {
      value: {
        handler() {
          this.$nextTick(() => {
            if(this.value) {
              if(this.parent) {
                this.element = document.querySelector(`[data-id="${this._uid}"]`);
                this.element.parentElement.style.position = 'relative';
              }
            }
          })
        }, deep: true, immediate: true
      }
    }
    }
</script>

<style lang="css" scoped>
.base-loading-overlay {
  position: absolute;
  z-index: 999999999;
  background: rgba(255,255,255,.75);
  backdrop-filter: blur(10px);
  top: 0;
  left: 0;
  width: 100%;
  display: flex;
  height: 100%;
  justify-content: center;
  align-items: center;
}
.base-loading-indication {
    margin-top: 20px;
    font-size: 13px;
    padding-left: 5px;
}

.base-loading-dots span {
    animation-name: dotBlink;
    animation-duration: 1.4s;
    animation-iteration-count: infinite;
    animation-fill-mode: both;
}
.base-loading-dots span:nth-child(2) {
    animation-delay: 0.2s;
}
.base-loading-dots span:nth-child(3) {
    animation-delay: 0.4s;
}

@keyframes dotBlink {
    0% {
        opacity: 0;
    }
    20% {
        opacity: 1;
    }
    100% {
        opacity: 0;
    }
}

.fade-in {
  animation: fadeIn 1s;
  -webkit-animation: fadeIn 1s;
  -moz-animation: fadeIn 1s;
  -o-animation: fadeIn 1s;
  -ms-animation: fadeIn 1s;
}
@keyframes fadeIn {
  0% {opacity:0;}
  100% {opacity:1;}
}

@-moz-keyframes fadeIn {
  0% {opacity:0;}
  100% {opacity:1;}
}

@-webkit-keyframes fadeIn {
  0% {opacity:0;}
  100% {opacity:1;}
}

@-o-keyframes fadeIn {
  0% {opacity:0;}
  100% {opacity:1;}
}

@-ms-keyframes fadeIn {
  0% {opacity:0;}
  100% {opacity:1;}
}
</style>
