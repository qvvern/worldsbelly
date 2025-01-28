<template>
    <div class="flex justify-center w-full text-center p-5 mt-10">
        <div class="mt-10">
            <NoResults
                title="Oops... looks like you got lost."
                @OnInteraction="goBack()"
            />
        </div>
    </div>
</template>

<script>
export default {
    name: '404',
    components: {
        NoResults: () => import('@/components/app/AppNoResults.vue'),
    },
    data() {
        return {
        }
    },
    methods: {
        goBack() {
            this.$router.go(-1);
        }
    }
}
</script>
