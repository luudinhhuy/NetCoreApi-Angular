import { Compiler, Directive, Inject, Injector, Input, OnInit, Type, ViewContainerRef, NgModuleFactory } from '@angular/core';
import { LoadModuleService } from '../services';
import { LAZY_MODULES_MAP, ILazyModules } from '../../load-module-map';

type ModuleWithRoot = Type<any> & { rootComponent: Type<any> };


@Directive({
    selector: '[loadModule]'
})
export class LoadModuleDirective implements OnInit {
    @Input('loadModule') moduleName: keyof ILazyModules;

    constructor(
        private _vcr: ViewContainerRef,
        private _injector: Injector,
        private _compiler: Compiler,
        private _loadModuleService: LoadModuleService,
        @Inject(LAZY_MODULES_MAP) private modulesMap: ILazyModules
    ) {
    }

    async ngOnInit() {
        let ref = this._loadModuleService.moduleRefs[this.moduleName];
        let refPromise = null;
        if (ref) {
            refPromise = Promise.resolve(ref);
        } else {
            const moduleFactory = await loadModuleFactory(
                this.modulesMap[this.moduleName] as () => Promise<any>, this._compiler
            );
            const moduleRef = moduleFactory.create(this._injector);

            ref = { moduleRef, moduleFactory };

            this._loadModuleService.updateModuleRefs(prev => ({
                ...prev,
                [this.moduleName]: ref
            }));
            refPromise = Promise.resolve(ref);
        }

        const { moduleFactory, moduleRef } = await refPromise;

        const rootComponent = (moduleFactory.moduleType as ModuleWithRoot).rootComponent;
        const factory = moduleRef.componentFactoryResolver.resolveComponentFactory(rootComponent);
        this._vcr.createComponent(factory);
    }
}

async function loadModuleFactory(loadChildren: () => Promise<any>, compiler: Compiler): Promise<NgModuleFactory<any>> {
    const t = await loadChildren();
    if (t instanceof NgModuleFactory) {
        return t;
    } else {
        return compiler.compileModuleAsync(t);
    }
}
