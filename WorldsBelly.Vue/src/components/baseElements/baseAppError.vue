<template>
  <baseDialog v-model="isOpen" width="800px" v-if="message">
    <template slot="header">
      Sorry we could not make your request.
    </template>
    <div slot="body" class="p-4" v-html="message">
    </div>
    <template slot="footer">
      <baseButton tertiary red @click.prevent.native="isOpen = false">Continue</baseButton>
    </template>
  </baseDialog>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";

export default {
    name: "baseAppError",
    data() {
        return {
          isOpen: true,
          message: null,
        };
    },
    computed: {
        ...mapGetters('app/', [
            'get_appError',
        ]),
    },
    methods: {
        ...mapMutations('app/', [
            'reset_appError',
        ]),
        close() {
            this.reset_appError();
        },
    },
    created() {
      if(this.get_appError?.response?.data?.errors) {
        var propterynames = Object.getOwnPropertyNames(this.get_appError?.response?.data?.errors);
        if(propterynames.some(_ => _.includes("ingredientLists"))) {
          this.message = "Make sure your ingredient list is filled out properly."
        }
      }
      else if(this.get_appError?.response?.data?.Message){
        this.message = this.get_appError?.response?.data?.Message;
      }
      else {
        this.isOpen = false;
      }
    },
    watch: {
      isOpen() {
        if(this.isOpen == false) {
            this.message = null;
            this.reset_appError();
        }
      }
    }
};
</script>
