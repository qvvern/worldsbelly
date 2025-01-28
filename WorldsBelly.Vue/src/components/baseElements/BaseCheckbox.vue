<template>
  <div class="base-checkbox">
    <label class="control control--checkbox">{{ label }}
      <input type="checkbox" :checked="value" @input="$emit('input', !value)" />
      <div class="control__indicator"></div>
    </label>
  </div>
</template>

<script>
  export default {
      name: "baseButton",
      props: {
        value: [Boolean],
        label: [String, Number],
      }
  }
</script>

<style lang="scss" scoped>
.base-checkbox {
.control {
	display: block;
	position: relative;
	padding-left: 25px;
  padding-top: 2px;
	cursor: pointer;
	font-size: 12px;
	input {
		position: absolute;
		z-index: -1;
		opacity: 0;
		&:checked {
			& ~ .control__indicator {
				background: #1cc319;
				&:after {
					display: block;
				}
			}
		}
		&:disabled {
			& ~ .control__indicator {
				background: #e6e6e6;
				opacity: 0.6;
				pointer-events: none;
			}
		}
	}
}
.control__indicator {
	position: absolute;
	top: 2px;
	left: 0;
	height: 16px;
	width: 16px;
	background: #e6e6e6;
	&:after {
		content: '';
		position: absolute;
		display: none;
	}
}
.control:hover input ~ .control__indicator,
.control input:focus ~ .control__indicator {
	background: #ccc;
}
.control:hover input:not([disabled]):checked ~ .control__indicator,
.control input:checked:focus ~ .control__indicator {
				background: #1cc319;
}
.control--checkbox {
	.control__indicator {
		&:after {
			left: 7px;
			top: 4px;
			width: 3px;
			height: 8px;
			border: solid #fff;
			border-width: 0 2px 2px 0;
			transform: rotate(45deg);
		}
	}
	input {
		&:disabled {
			& ~ .control__indicator {
				&:after {
					border-color: #7b7b7b;
				}
			}
		}
	}
}

}
</style>
