<template>
  <baseLayout class="admin-side-content">
    <div slot="header" class="admin-side-content__header">
      <h4>Create recipe</h4>
    </div>
    <div slot="content" class="admin-side-content__body">
      <template v-if="isActive('RecipeCreate')">
        <Stepper v-if="$route.params.id" />
        <div v-else class="p-4">
          Please select a recipe type
        </div>
      </template>
    </div>
  </baseLayout>
</template>

<script>
import { administrationMixin } from "@/mixins/AdministrationMixin";

export default {
	name: 'SideMenu',
  mixins: [administrationMixin],
	components: {
		Stepper: () => import('@/components/administration/Stepper.vue'),
	},
	data() {
		return {
		}
	},
  computed: {
  },
}
</script>

<style lang="scss" scoped>
.admin-side-content {
  width: 100%;
  // background: #f5f5f5 !important;
      &__header {
        padding: 15px;
        font-size: 14px;
        font-weight: 500;
        text-align: left;
        line-height: 1.22;
        display: block;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        border-bottom: 1px solid #e7e7e7;
        h4 {
          font-family: sans-serif;
          -webkit-font-smoothing: antialiased;
          text-rendering: optimizeLegibility;
          font-size: 16px;
          font-weight: 600;
          margin: 0;
          padding: 0;
          box-sizing: inherit;
          transition: color 0.3s ease-in-out;
          text-decoration: none;
          color: #2e2e2e;
        }
      }
      &__body {
        height: auto;
        width: 100%;
          font-family: sans-serif;
          -webkit-font-smoothing: antialiased;
          text-rendering: optimizeLegibility;
          font-size: 14px;
          font-weight: 500;
          margin: 0;
          padding: 0;
          box-sizing: inherit;
          transition: color 0.3s ease-in-out;
          text-decoration: none;
          color: #64686A;
      }
    }
</style>
