
import { Component, signal, computed, effect } from '@angular/core';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {

  // 1. Signal: Stores the counter value
  count = signal(0);

  // 2. Computed Signal: Derives the counter status
  counterStatus = computed(() => {
    const value = this.count();

    if (value > 0) {
      return 'Positive';
    } else if (value < 0) {
      return 'Negative';
    } else {
      return 'Zero';
    }
  });

  // 3. Effect: Runs whenever count changes
  constructor() {
    effect(() => {
      console.log('Counter value changed:', this.count());
    });
  }

  // Increment
  increment() {
    this.count.update(value => value + 1);
  }

  // Decrement
  decrement() {
    this.count.update(value => value - 1);
  }

  // Reset
  reset() {
    this.count.set(0);
  }
}
