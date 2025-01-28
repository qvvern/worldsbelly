<template>
<span></span>
</template>
<script>
import LeaderLine from 'leader-line-vue';

export default {
    name: "BaseLine",
    props: {
      container: String,
      from: String,
      to: String,
    },
    data() {
        return {
            line: null,
            lineElement: null,
            linksContainer: null,
            scrollElement: null,
        };
    },
    beforeDestroy() {
        this.removeLeaderLine();
    },
    mounted() {
        this.$nextTick(() => {
            this.linksContainer = document.querySelector(this.container);
            this.addLeaderLine();
        })
    },
    methods: {
        handleLine(e) {
          this.line.position();
          var rectWrapper = this.linksContainer.getBoundingClientRect();
          this.lineElement.style.transform = `translate(${-rectWrapper.left + pageXOffset}px, ${-rectWrapper.top + pageYOffset}px)`;
        },
        removeLeaderLine() {
            var leaderLine = this.linksContainer?.querySelector(`[data-id="${this.line._id}"]`);
            if(leaderLine) {
                document.body.appendChild(leaderLine);
            }
            this.line.remove();
            this.removeMoveListener();
        },
        addLeaderLine() {
            const Vue = this;
                this.line = LeaderLine.setLine(
                        document.querySelector(Vue.from),
                        document.querySelector(Vue.to),
                  {
                    hide: true,
                    startSocket: 'bottom',
                    endSocket: 'top'
                  }
                );
                this.lineElement = document.querySelector('body > .leader-line');
                this.lineElement.setAttribute('data-id', this.line._id);
                this.linksContainer?.appendChild(this.lineElement);
                // this.linksContainer?.querySelector(`[data-id="${this.line._id}"] > defs > .leader-line-line-path`).addEventListener("mousedown", (e)=>{ Vue.handleLineClick(e) });
                this.line.position();
                this.$nextTick(() => {
                  this.line.show('draw');
                })
                this.addMoveListener();
        },
        addMoveListener() {
            this.scrollElement = document.querySelector('.recipe-create .base-layout-flex-row__content-inner__content');
            this.scrollElement.addEventListener('scroll', this.handleLine);
        },
        removeMoveListener() {
          this.scrollElement.removeEventListener('scroll', this.handleLine);
        },
        beforeDestroy() {
          this.removeLeaderLine();
        }
    },
    watch: {
    }
};
</script>

<style lang="css">
.designer-step-canvas-links > defs *, .leader-line-line-path {
    pointer-events: all !important;
}
.leader-line g {
    cursor: pointer;
}
.leader-line text {
    font-size: 12px !important;
    fill: #000 !important;
    stroke: none !important;
}
</style>
<style lang="scss" scoped>
#step-linking-start {
    opacity: 0;
    width:1px;
    height:1px;
    // background: red;
    position:fixed;
    pointer-events: none; /* Allow clicking trough the div */
}
.step {
    // margin: 0;
    box-sizing: border-box;
    z-index: 1;
    cursor: pointer;

    .step-wrapper {
        display: flex;
        // flex-direction: column;
        // justify-content: center;
        // align-items: center;
        border-radius: 6px;
        padding: 16px;
        position: relative;
        min-width: 250px;
        max-width: 300px;
        border: 1px solid #e3e3e3;
        background: white !important;
        box-shadow: 0px 0px 3px 0px rgba(0,0,0,.2);
        &-overlay {
            position: absolute;
            z-index: 10;
            top: -1px;
            left: -1px;
            right: -1px;
            bottom: -1px;
            background-color: rgba(#029aeb, 0.25);
            border: 2px solid #029aeb;
            border-radius: 6px;
        }
    }

    &-box {
        display: flex;
        justify-content: left;
        align-items: center;

        &__icon {
            text-align: center;
            display: flex;
            justify-content: center;
            align-items: center;
            width: fit-content;
            height: fit-content;
            border-radius: 100%;
            border: 1px solid #e3e3e3;
            padding: 10px;
        }

        &__status {
            position: absolute;
            top: 0;
            right: 0;
            transform: translate(50%, -50%);
        }
    }

    .step-label,
    .step-label * {
        // margin: 0 auto;
        // width: fit-content;
        text-align: left;
        color: #000;
        font-size: 14px;
        font-weight: 600;
        word-break: break-word;
    }
    .step-titel {
      font-weight: 500;
      font-size: 11px;
      line-height: 140%;
      color: rgb(114, 110, 134);
      text-align: left;
      text-transform: uppercase;

    }
}
</style>
