<template>
  <div class="studio-page">
    <div class="studio-page-sidebar">
      <div class="studio-page-sidebar__bar">
          <SideMenu />
      </div>
    </div>
    <div class="studio-page-content">
        <router-view />
    </div>
  </div>
</template>

<script>

export default {
	name: 'Administration',
	components: {
		SideMenu: () => import('@/components/administration/SideMenu.vue'),
	},
	data() {
		return {
		}
	},
}
</script>

<style lang="scss" scoped>
.studio-page {
  height: 100%;
  width: 100%;
  display: flex;
  &-sidebar {
    height: 100%;
    background: #FAFBFC;
    border-right: 1px solid var(--border-color);
    position: relative;
    overflow-x: hidden;
    overflow-y: auto;
    &__bar {
      height: 100%;
      width: 100%;
      display: flex;
    }
  }
  &-content {
    width: 100%;
    height: 100%;
  }
}
</style>
