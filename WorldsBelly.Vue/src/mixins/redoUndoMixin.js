import _ from "lodash";
export const redoUndoMixin = {
    data() {
        return {
            undos: [],
            redos: [],
            redoUndoItem: null,
            redoAvailable: false,
            undoAvailable: false,
        };
    },
    methods: {
        undoHandler() {
            this.redos.push(_.cloneDeep(this.undos.pop()));
            this.redoUndoItem = _.cloneDeep(this.undos.pop());
        },
        redoHandler() {
            let item = this.redos.pop();
            this.redoUndoItem = _.cloneDeep(item);
        }
    },
    watch: {
        undos: {
            handler() {
                if(this.undos.length > 1) {
                    this.undoAvailable = true;
                    return;
                }
                this.undoAvailable = false;
            },
            deep: true
        },
        redos: {
            handler() {
                if(this.redos.length > 0) {
                    this.redoAvailable = true;
                }
                else {
                    this.redoAvailable = false;
                }
            },
            deep: true
        }
    }
};
