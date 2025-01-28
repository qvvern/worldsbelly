<template>
  <swiper ref="mySwiper" v-if="id" class="base-carousel swiper" :options="swiperOptions" :style="{width: width, height: height}"  @slideChange="runOnChange">
    <slot></slot>
    <div v-if="pagination" :class="`swiper-pagination swiper-pagination${id}`" slot="pagination" :style="{'padding-top': paginationOffsetTop}"></div>
    <template v-if="navigation">
        <div :class="'swiper-button-prev swiper-button-prev'+id" slot="button-prev"></div>
        <div :class="'swiper-button-next swiper-button-next'+id" slot="button-next"></div>
    </template>
  </swiper>
</template>

<script>

export default {
    /* documentation here: https://v1.github.surmon.me/vue-awesome-swiper */
    name: 'BaseCarousel',
    props: {
        width: {
            type: [String],
            default: '100%'
        },
        height: {
            type: [String],
            default: 'fit-content'
        },
        navigation: {
            type: [Boolean],
            default: false
        },
        paginationOffsetTop: {
            type: [Number],
            default: 0
        },
        pagination: {
            type: [Boolean],
            default: true
        },
        loop: {
            type: [Boolean],
            default: false
        },
        centeredSlides: {
            type: [Boolean],
            default: true
        },
        autoplay: {
            type: [Boolean],
            default: false
        },
        autoplayDelay: {
            type: [Number],
            default: 3000
        },
        autoplayDisableOnInteraction: {
            type: [Boolean],
            default: false
        },
        slidesPerView: {
            type: [Number],
            default: 1
        },
        slidesPerColumn: {
            type: [Number],
            default: 1
        },
        freeMode: {
            type: [Boolean],
            default: false
        },
        spaceBetween: {
            type: [Number],
            default: 0
        },
        direction: {
          type: String,
          default: 'horizontal'
        },
    },
	data() {
		return {
			id: null
		};
	},
    beforeMount() {
        this.id = this._uid;
    },
    computed: {
        swiperOptions() {
            return {
                direction: this.direction,
                centeredSlides: this.centeredSlides,
                autoplay: this.autoplay ? {
                    delay: this.autoplayDelay,
                    disableOnInteraction: this.autoplayDisableOnInteraction
                } : false,
                slidesPerView: this.slidesPerView,
                slidesPerColumn: this.slidesPerColumn,
                spaceBetween: this.spaceBetween,
                freeMode: this.freeMode,
                loop: this.loop,
                pagination: {
                    el: '.swiper-pagination'+this.id,
                    clickable: true
                },
                navigation: {
                    nextEl: '.swiper-button-next'+this.id,
                    prevEl: '.swiper-button-prev'+this.id
                },
            }
        }
    },
    methods: {
      runOnChange() {
        this.$emit('activeSlide', this.$refs.mySwiper.$swiper.activeIndex);
      },
      toSlide(index) {
        this.$refs.mySwiper.$swiper.slideTo(index, 0)
      },
    }
}
</script>
<style lang="scss">
.base-carousel {
  .swiper-pagination {
    pointer-events: none !important;
    .swiper-pagination-bullet {
      pointer-events: auto !important;
    }
  }
}
</style>
