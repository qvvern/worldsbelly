<template>
    <swiper-slide :style="{width: width, height:height}" class="base-carousel-slide"><slot></slot></swiper-slide>
</template>

<script>

export default {
    /* documentation here: https://v1.github.surmon.me/vue-awesome-swiper */
    name: 'BaseCarouselSlide',
    props: {
        width: {
            type: [String],
            default: 'fit-content'
        },
        height: {
            type: [String],
            default: 'fit-content'
        },
    }
}
</script>
