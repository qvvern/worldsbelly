<template>
  <div class="flex flex-wrap">
    <div class="flex flex-wrap">
      <template v-for="(tag, u) in value">
        <v-chip :key="u" class="mr-2"  small
        close
        @click:close="remove(tag, u)">{{tag.name}}</v-chip>
      </template>
    </div>

      <v-menu
        v-model="menu"
        transition="slide-x-transition"
        bottom
        right
        :max-width="255"
        :close-on-content-click="false"
      >
        <template v-slot:activator="{ on, attrs }">
          <div class="flex items-center cursor-pointer"
            v-bind="attrs"
            v-on="on">
            <baseIcon size="14">icofont-plus-circle</baseIcon>
            <p style="font-size: 12px" class="pl-1">Add tag</p>
          </div>
        </template>
      <v-card>

      <v-list-item three-line>
        <v-list-item-content class="my-0">
          <div class="text-overline">
            Add Tags
          </div>
          <div class="py-0 px-1" style="height: 100px">
            <div class="w-full">
                <v-chip
                    v-for="(tag, i) in addTags"
                    :key="i"
                    small
                    class="mb-1"
                    @click:close="removeTag(tag)"
                    close
                    >{{ tag.name }}</v-chip
                >
            </div>
            <FilterSelect
                :items="customTags"
                :excludeItems="value.concat(addTags)"
                @selected="selectTag($event)"
                @search="getSuggestedTags($event)"
                @searchMore="getSuggestedTagsMore($event)"
                :moreLimit="50"
                :loading.sync="loadingTags"
                itemText="name"
                itemValue="name"
                placeholder="Type tag"
                maxHeight="100px"
            />
            <!-- <baseSelect
              v-model="tagToAdd"
              :items.sync="get_custom_tags"
              @input="addTag()"
              :moreLimit="50"
              :loading="loadingTags"
              itemText="name"
              itemValue="id"
              placeholder="Type tag"
              input
              solo
              flat
              small
              shadow
              returnAll
              :autofocus="true"
            /> -->
          </div>
        </v-list-item-content>
      </v-list-item>
        <v-card-actions class="flex justify-center mt-0 pt-0 mr-3 pb-3">
          <v-btn
            outlined
            text
            @click="close()"
          >
            Cancel
          </v-btn>
          <v-btn
            outlined
            text
            @click="addTag()"
            :disabled="!addTags || addTags.length == 0"
          >
            Add
          </v-btn>
        </v-card-actions>
      </v-card>
      </v-menu>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex'
import _ from "lodash";
  export default {
      name: "RecipeDraftsCustomTags",
      components: {
        FilterSelect: () => import("@/components/filter/FilterSelect.vue"),
      },
      props: {
        value: Array,
      },
      data() {
        return {
          addTags: [],
          // tagToAdd: null,
          loadingTags: false,
          menu: false,
          searchText: null
        }
      },
      computed: {
        ...mapGetters('customTag/', ['get_custom_tags']),
        customTags() {
          var searchTag = {id:0, name: this.searchText};
          var isFound = this.get_custom_tags.some(_ => _.name?.toLowerCase()?.trim() == this.searchText?.toLowerCase()?.trim());
          if(isFound || this.searchText?.length <= 2) {
            return this.get_custom_tags
          }
          return [searchTag].concat(this.get_custom_tags)
        }
      },
      methods: {
		...mapActions('customTag/', ['fetch_custom_tags', 'append_custom_tags']),
        // tagExists() {
        //   return this.value?.some(_ => _.name == this.tagToAdd?.name)
        // },
        getSuggestedTags(val) {
            this.searchText = val;
            this.loadingTags;
            this.fetch_custom_tags(val)
            .finally(() => (this.loadingTags = false));
        },
        getSuggestedTagsMore(val) {
            this.loadingTags;
            this.append_custom_tags(val)
            .finally(() => (this.loadingTags = false));
        },
        selectTag(tag) {
          this.addTags.push(tag);
        },
        removeTag(tag) {
           this.addTags = this.addTags.filter(_ => _.name != tag.name);
        },
        // addTag() {
        //   if(this.tagExists()) {
        //     this.searchText = this.tagToAdd?.name;
        //     this.tagToAdd = null;
        //     return;
        //   }
        //   var clonedval = _.cloneDeep(this.value);
        //   if(this.tagToAdd != null) {
        //     clonedval.push(this.tagToAdd);
        //   }
        //   else {
        //     clonedval.push({id:0, name: this.searchText});
        //   }
        //   this.$emit('input', clonedval);
        //   this.close();
        // },
        addTag() {
          var clonedval = _.cloneDeep(this.value);
          var both = clonedval.concat(this.addTags);
          this.$emit('input', both);
          this.close();
        },
        remove(item, index) {
          var clonedval = _.cloneDeep(this.value);
          clonedval.splice(index, 1);
          this.$emit('input', clonedval);
        },
        close() {
          this.addTags = [];
          this.searchText = null;
          // this.tagToAdd = null;
          this.menu = false;
        }
      },
    created() {
          if(this.get_custom_tags.length > 1) {
            this.isPageReady = true;
            return;
          }
          this.fetch_custom_tags().finally(() => {
              this.isPageReady = true;
          });
    },
  };
</script>

<style lang="scss" scoped>
.recipe-drafts-edit-actions {
}
</style>
