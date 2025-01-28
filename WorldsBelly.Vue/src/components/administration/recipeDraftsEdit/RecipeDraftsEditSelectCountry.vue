
<template>
  <baseSelect
      labelTop
      class="country-select"
      :value="value"
      @input="$emit('input', $event)"
      @search="search($event)"
      :items="filteredCountries"
      itemValue="id"
      height="55px"
      itemText="name"
      :label="$t('cuisines')"
      :placeholder="`${$t('select')} ${$t('country')}`"
      costumized
      outlined
      right
    >
    <template v-slot:selection="data">
        <v-avatar left v-if="data.item.id != 0">
          <country-flag :country='data.item.code'/>
        </v-avatar>
        {{ data.item.name }}
    </template>

    <template v-slot:item="{ item }">
        <v-list-item-avatar left>
          <country-flag :country='item.code' />
        </v-list-item-avatar>
        <v-list-item-content v-text="item.name"></v-list-item-content>
    </template>
  </baseSelect>
</template>

<script>
import { mapGetters } from 'vuex'
import _ from "lodash";

export default {
	name: 'RecipeDraftsEditSelectCountry',
  props: {
    value: Number,
  },
	data() {
		return {
      country: 20,
      defaultItem: {id: 0,name: "test",code:"f"},
      searchText: null
		}
	},
	components: {
		CountryFlag: () => import('vue-country-flag'),
	},
  computed: {
		...mapGetters('country/', ['get_countries']),
    filteredCountries() {
      if(this.searchText) {
        return this.get_countries.filter(_ => _.name?.toLowerCase().includes(this.searchText?.toLowerCase()))
      }
      else {
        return this.get_countries
      }
    }
  },
  methods: {
    search(text) {
      this.searchText = text;
    }
  }
}
</script>

<style lang="scss">
.country-select {
    // float: right;
  //   // height: 40px;
  // .v-input__slot {
  //   padding: 0 !important;
  // }
}
</style>
