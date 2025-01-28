<template>
    <div class="recipe-stepper">
        <div class="recipe-stepper-header">
            <!-- <v-icon>icofont-rounded-left</v-icon> -->
            <h3>Create Recipe</h3>
        </div>
        <v-stepper v-model="step" vertical>
            <v-stepper-step step="1"> Type of recipe </v-stepper-step>
            <!-- <v-stepper-content step="1">
                <v-stepper class="recipe-stepper-two" v-model="setupIndex" vertical>
                    <v-stepper-step v-for="(step, i) in steps" :step="step.id" :key="i" :active="false">
                        {{ step.menuDescription }}
                        <template v-if="step.selectedName"
                            ><b>{{ step.selectedName }}</b></template
                        >
                    </v-stepper-step>
                </v-stepper>
            </v-stepper-content> -->

            <v-stepper-step step="2"> Age </v-stepper-step>
            <!-- <v-stepper-content step="2">
                <v-stepper class="recipe-stepper-two" v-model="setupIndex" vertical>
                    <v-stepper-step v-for="(step, i) in steps" :step="step.id" :key="i" :active="false">
                        {{ step.menuDescription }}
                        <template v-if="step.selectedName"
                            ><b>{{ step.selectedName }}</b></template
                        >
                    </v-stepper-step>
                </v-stepper>
            </v-stepper-content> -->

            <v-stepper-step step="3"> Season </v-stepper-step>
            <!-- <v-stepper-content step="3">
                <v-stepper class="recipe-stepper-two" v-model="setupIndex" vertical>
                    <v-stepper-step v-for="(step, i) in steps" :step="step.id" :key="i" :active="false">
                        {{ step.menuDescription }}
                        <template v-if="step.selectedName"
                            ><b>{{ step.selectedName }}</b></template
                        >
                    </v-stepper-step>
                </v-stepper>
            </v-stepper-content> -->

            <v-stepper-step step="4"> Categories </v-stepper-step>
            <v-stepper-content step="4">
                <v-stepper class="recipe-stepper-two" v-model="setupIndex" vertical>
                    <v-stepper-step v-for="(step, i) in steps" :step="step.id" :key="i" :active="false">
                        {{ step.menuDescription }}
                        <template v-if="step.selectedName"
                            ><b>{{ step.selectedName }}</b></template
                        >
                    </v-stepper-step>
                </v-stepper>
            </v-stepper-content>

            <v-stepper-step step="5"> Create </v-stepper-step>
            <!-- <v-stepper-content step="5">
                <v-stepper class="recipe-stepper-two" v-model="setupIndex" vertical>
                    <v-stepper-step v-for="(step, i) in steps" :step="step.id" :key="i" :active="false">
                        {{ step.menuDescription }}
                        <template v-if="step.selectedName"
                            ><b>{{ step.selectedName }}</b></template
                        >
                    </v-stepper-step>
                </v-stepper>
            </v-stepper-content> -->
        </v-stepper>
        <baseButton tertiary small class="mx-4" :disabled="setupIndex == 1 && step == 1" @click.prevent.native="$emit('prevSetup', setupIndex)">
            Previous
        </baseButton>
    </div>
</template>
<script>
import { mapGetters } from "vuex";
export default {
    name: "Stepper",
    props: {
        steps: Array,
        step: Number,
        selectedTags: Array,
    },
    data() {
        return {
            setupIndex: 1,
        };
    },
    computed: {
        ...mapGetters("tagSelectable/", ["get_tag_selectables"]),
    },
    watch: {
        steps: {
            handler() {
                this.setupIndex = this.steps.find((_) => _.active == true)?.id;
                if(this.steps[this.setupIndex-1]?.excludedTags?.length > 0 && this.steps[this.setupIndex-1].excludedTags?.some(tag => this.selectedTags.includes(Number(tag)))) {
                  this.$emit('nextSetup');
                }
            },
            deep: true,
        },
    },
};
</script>

<style lang="scss">
.recipe-stepper {
    height: 100%;
    width: 300px;
    box-shadow: 0px 0px 10px -1px rgba(0, 0, 0, 0.4);
    &-header {
        padding: 10px;
        padding-top: 20px;
        display: flex;
        align-items: center;
        h3 {
            text-align: left;
            margin-top: 0;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.1rem;
            font-weight: 500;
            color: #868686;
            margin-left: 10px;
        }
        .v-icon {
            color: #4e4e4e;
        }
    }
    .v-stepper {
        background: transparent !important;
        border-radius: 0;
        box-shadow: none !important;
        &__label {
            border: none;
            font-size: 14px;
            font-weight: 400;
            color: #0d0c22;
        }
        &__content {
            padding-right: 0px !important;
        }
        .recipe-stepper-two {
            padding: 0;
            padding-right: 20px !important;
            .v-stepper__step {
                padding: 8px;
                border-radius: 20px;
                background: rgba(255, 255, 255, 0.9);
                font-weight: 400;
                margin-bottom: 5px;
                .v-stepper__label {
                    font-weight: 400;
                    font-size: 12px !important;
                }
                &--active {
                    background-color: orange !important;
                    .v-stepper__label {
                        font-weight: 500;
                        color: white !important;
                        text-shadow: none;
                    }
                    .v-stepper__step__step {
                        content: "";
                        background-color: white !important;
                        border-color: white !important;
                        color: white !important;
                    }
                }
                &__step {
                    height: 14px;
                    min-width: 14px;
                    width: 14px;
                    color: transparent;
                }
            }
        }
    }
}
</style>
